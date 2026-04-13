using System.Collections.Generic;
using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class ArchiveHeroUpdate_UIArchiveRefresh : AEvent<Scene, ArchiveHeroUpdate>
    {
        protected override async ETTask Run(Scene scene, ArchiveHeroUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIArchive);
            if (ui == null)
            {
                return;
            }

            UIArchiveComponent uiArchiveComponent = ui.GetComponent<UIArchiveComponent>();
            uiArchiveComponent.SetShowType(uiArchiveComponent.CurrentPage);

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIArchiveComponent))]
    [FriendOf(typeof(UIArchiveComponent))]
    public static partial class UIArchiveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIArchiveComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Reward = rc.Get<GameObject>("Button_Reward").GetComponent<Button>();
            self.Button_Type_Hero = rc.Get<GameObject>("Button_Type_Hero").GetComponent<Button>();
            self.Content_UIArchiveHeroItem = rc.Get<GameObject>("Content_UIArchiveHeroItem").transform;
            self.UIArchiveHeroItem = rc.Get<GameObject>("UIArchiveHeroItem");
            self.UIArchiveHeroItem.gameObject.SetActive(false);
            self.Text_CollectProgress = rc.Get<GameObject>("Text_CollectProgress").GetComponent<TMP_Text>();
            self.Dotween_Upper = rc.Get<GameObject>("Dotween_Upper").transform;
            self.Dotween_Under = rc.Get<GameObject>("Dotween_Under").transform;

            self.Button_Close.AddListener(() => { self.OnClose(); });
            self.Button_Reward.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIArchiveReward).Coroutine(); });
            self.Button_Type_Hero.onClick.AddListener(() => { self.SetShowType(0); });

            self.SetShowType(0);
        }

        [EntitySystem]
        private static void Destroy(this UIArchiveComponent self)
        {
            self.UIArchiveHeroItemList.Clear();
            self.UIArchiveHeroItem = null;
        }

        public static void SetShowType(this UIArchiveComponent self, int page)
        {
            self.CurrentPage = page;

            self.Button_Type_Hero.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_Hero.transform.Find("Image_Off").gameObject.SetActive(page != 0);

            self.UpdateHeroList(page);
        }

        private static void UpdateHeroList(this UIArchiveComponent self, int page)
        {
            ArchiveComponentC archiveComponent = self.Root().GetComponent<ArchiveComponentC>();
            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();

            int heroCount = archiveComponent.ArchiveHeroList.Count;
            int allHeroCount = HeroConfigCategory.Instance.DataMap.Count;

            self.Text_CollectProgress.SetTextFormat("{0}/{1}", heroCount, allHeroCount);

            List<EntityRef<ArchiveHero>> archiveHeroeList = null;
            if (page == 0)
            {
                archiveHeroeList = archiveComponent.ArchiveHeroList;
            }
            else
            {
                return;
            }

            List<int> notHaveHeroConfigIdList = new();
            foreach (HeroConfig config in HeroConfigCategory.Instance.DataList)
            {
                bool exist = false;
                foreach (ArchiveHero archiveHero in archiveHeroeList)
                {
                    if (archiveHero.HeroConfigId == config.Id)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    continue;
                }

                notHaveHeroConfigIdList.Add(config.Id);
            }

            while (self.UIArchiveHeroItemList.Count < archiveHeroeList.Count + notHaveHeroConfigIdList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIArchiveHeroItem, self.Content_UIArchiveHeroItem);
                UIArchiveHeroItem newItem = self.AddChild<UIArchiveHeroItem, GameObject>(go);
                self.UIArchiveHeroItemList.Add(newItem);
            }

            for (int i = 0; i < archiveHeroeList.Count; i++)
            {
                self.UIArchiveHeroItemList[i].UpdateInfo(archiveHeroeList[i]).Coroutine();
                self.UIArchiveHeroItemList[i].GameObject.SetActive(true);
            }

            for (int i = archiveHeroeList.Count; i < archiveHeroeList.Count + notHaveHeroConfigIdList.Count; i++)
            {
                self.UIArchiveHeroItemList[i].UpdateInfo(notHaveHeroConfigIdList[i - archiveHeroeList.Count]).Coroutine();
                self.UIArchiveHeroItemList[i].GameObject.SetActive(true);
            }

            for (int i = archiveHeroeList.Count + notHaveHeroConfigIdList.Count; i < self.UIArchiveHeroItemList.Count; i++)
            {
                self.UIArchiveHeroItemList[i].GameObject.SetActive(false);
            }
        }
        
        private static void OnClose(this UIArchiveComponent self)
        {
            self.Dotween_Under.DOLocalMoveY(-700, 0.2f);
            self.Dotween_Upper.DOLocalMoveY(700, 0.2f).OnComplete(() => self.Root().GetComponent<UIComponent>().Remove(UIType.UIArchive));
        }
    }
}