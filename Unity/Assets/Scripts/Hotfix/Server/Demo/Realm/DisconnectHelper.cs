using System;
using System.Collections.Generic;

namespace ET.Server
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);

            if (self.InstanceId != instanceId)
            {
                return;
            }

            self.Dispose();
        }

        public static async ETTask KickPlayerNoLock(Player player, int type)
        { 
            if (player == null || player.IsDisposed)
            {
                return;
            }
            Log.Console($"KickPlayerNoLock:  {player.Id}  {type}");
            switch (player.PlayerState)
            {
                case PlayerState.Disconnect:
                    break;
                case PlayerState.Gate:
                    break;
                case PlayerState.Game:
                    //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                    await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(player.Id, G2M_RequestExitGame.Create());
                    
                    // 通知邮件服下线
                    await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Mail).Call(player.Id, G2Mail_ExitMailServer.Create());
                    
                    // 通知聊天服下线
                    await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Chat).Call(player.Id, G2Chat_ExitChatServer.Create());
                    
                    // 通知好友服下线
                    await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Friend).Call(player.Id, G2Friend_ExitFriendServer.Create());
                    
                    //通知排行服下线
                    await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Rank).Call(player.Id, G2Rank_ExitRankServer.Create());

                    // //通知组队服
                    // await BroadCastHelper.SendServerMessage(player.Root(), UnitCacheHelper.GetTeamServerId(player.Zone()) , NoticeType.PlayerExit, player.UnitId.ToString());
                    
                    //通知Solo服
                    //await BroadCastHelper.SendServerMessage(player.SoloServerID, NoticeType.PlayerExit, player.UnitId.ToString());
                    
                    //通知移除账号角色登录信息
                    G2L_RemoveLoginRecord g2LRemoveLoginRecord = G2L_RemoveLoginRecord.Create();
                    g2LRemoveLoginRecord.AccountName = player.Account;
                    g2LRemoveLoginRecord.ServerId = player.Zone();
                    var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord) await player.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.LoginCenterConfig.ActorId, g2LRemoveLoginRecord);
                    
                    // await ExitWorldChatServer(player.Scene(), player.ChatInfoInstanceId);
                    // await ExitOtherServer(player.Scene(), player.Id);

                    break;
            }
    
            TimerComponent timerComponent = player.Root().GetComponent<TimerComponent>();
            player.PlayerState = PlayerState.Disconnect;
            
            await player.GetComponent<PlayerSessionComponent>().RemoveLocation(LocationType.GateSession);
            await player.RemoveLocation(LocationType.Player);
            
            // 不加这俩段的话，重连发送的第一条C2M服务器不处理，会报actor not found mailbox
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.GateSession)?.Remove(player.Id);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Unit)?.Remove(player.Id);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Mail)?.Remove(player.Id);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Chat)?.Remove(player.Id);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Friend)?.Remove(player.Id);
            player?.Root()?.GetComponent<MessageLocationSenderComponent>()?.Get(LocationType.Rank)?.Remove(player.Id);
            
            player.Root().GetComponent<PlayerComponent>()?.Remove(player);
            player?.Dispose();
    
            await timerComponent.WaitAsync(300);
        }

        public static async ETTask ExitOtherServer(Scene root, long unitId)
        {
            A2A_BroadcastSceneRequest broadcastSceneRequest = A2A_BroadcastSceneRequest.Create();
            broadcastSceneRequest.UnitId = unitId;
            List<StartSceneConfig> otherScenes = BroadCastHelper.GetAllScene(root.Zone());

            for (int i = 0; i < otherScenes.Count; i++)
            {
                await root.GetComponent<MessageSender>().Call(otherScenes[i].ActorId, broadcastSceneRequest);
            }
        }

        public static async ETTask KickPlayer(Player player, bool isException = false)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            long instanceId = player.InstanceId;

            CoroutineLockComponent coroutineLockComponent = player.Root().GetComponent<CoroutineLockComponent>();

            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, player.Account.GetLongHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                {
                    return;
                }
                await KickPlayerNoLock(player, 5);
            }
        }
        
    }
}