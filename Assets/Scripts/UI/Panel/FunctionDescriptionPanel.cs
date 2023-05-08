using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LXQJZ.UI
{
	public class FunctionDescriptionPanel : BasePanel<FunctionDescriptionPanel>
	{
		[SerializeField] Image imgFill;
		protected override void Start()
		{
			EventCenter.GetInstance().AddEventListener<float>("进度条更新", UpdateSlider);
			LoadMainScene();
		}
		void LoadMainScene()
		{
			ScenesMgr.GetInstance().LoadSceneAsyn("MainScene", LoadDebug);
		}
		void UpdateSlider(float value)
		{
			imgFill.fillAmount = value;
		}
		void LoadDebug()
		{
			Debug.Log("加载完成");
		}
	}
}

