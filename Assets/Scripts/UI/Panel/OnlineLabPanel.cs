using LXQJZ.Exam;
using LXQJZ.Task;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace LXQJZ.UI
{
	public class OnlineLabPanel : BasePanel<OnlineLabPanel>
	{
		[Header("���Ͻǰ�ť")]
		[SerializeField] Button btnGoBack;
		[SerializeField] Button btnSketch;
		[SerializeField] Button btnHelp;
		[SerializeField] Button btnFullScreen;
		[Header("���Ͻǰ�ť����")]
		[SerializeField] GameObject objGoBack;
		[SerializeField] Button btnGoBackConfirm;
		[SerializeField] Button btnGoBackCancle;
		[SerializeField] ScalePathAnim imgSketch;
		[SerializeField] List<Image> imgSketchs;
		[SerializeField] GameObject objHelp;
		[Header("����Toggle")]
		[SerializeField] GameObject stepObj;
		RectTransform stepRect;
		[SerializeField] List<Toggle> togList;
		int fatherIndex = 0;
		[Header("�Ӳ���Text Group")]
		[SerializeField] List<GameObject> objChildStepFatherList;
		[SerializeField] List<Text> txtChildList = new List<Text>();
		int childIndex = 0;
		[Header("����ʵ����ʾ")]
		[SerializeField] GameObject objOnlinLabTipHeader;
		[SerializeField] Image imgRetract;
		[SerializeField] Text txtOnlineLabTipHeader;
		[SerializeField] GameObject objOnlinLabTipContent;
		[SerializeField] Text txtOnlineLabTipContent;
		//[SerializeField] bool isNeedTitleTip = true;
		[Header("ʵ�鲽���һ����")]
		[SerializeField] InputField inputWaxTubeWidth;
		[SerializeField] GameObject onlineLabChoice;
		[SerializeField] Toggle togA;
		[SerializeField] Text txtA;
		[SerializeField] Toggle togB;
		[SerializeField] Text txtB;
		[SerializeField] Text txtOnlineLabTip;
		bool isA = false;
		bool isRight = false;

		//���ʵ������ʾ����
		[SerializeField] Button btnLabCompleted;
		[SerializeField] Image imgLabCompleted;

		void InitListener()
		{
			//�����������ʾ����
			btnLabCompleted.onClick.AddListener(() => { imgLabCompleted.gameObject.SetActive(false); });
			TaskManager.Instance.OnLabCompleted += () => { imgLabCompleted.gameObject.SetActive(true); };
			//���踸���������¼
			stepRect = stepObj.transform as RectTransform;
			//������ʾ
			objOnlinLabTipHeader.GetComponent<Button>().onClick.AddListener(SwitchOnlineLabTipContent);
			objOnlinLabTipContent.GetComponent<Button>().onClick.AddListener(SwitchOnlineLabTipContent);
			//���Ͻǰ�ť
			btnHelp.onClick.AddListener(SwitchHelp);
			btnGoBack.onClick.AddListener(ShowObjGoBack);
			btnFullScreen.onClick.AddListener(() => { Screen.fullScreen = !Screen.fullScreen; });
			//���Ͻǵ�����ť
			btnGoBackConfirm.onClick.AddListener(ShowMainPanel);
			btnGoBackCancle.onClick.AddListener(HideObjGoBack);
			//���Ʋ�ͼ��ť
			btnSketch.onClick.AddListener(SwitchSketch);
			//չ������Toggle
			for (int i = 0; i < togList.Count; i++)
			{
				int index = i;
				togList[i].onValueChanged.AddListener((isOn) =>
				{
					objChildStepFatherList[index].SetActive(isOn);
				});
			}

			togA.onValueChanged.AddListener(OnOnlineLabChoiceA);
			togB.onValueChanged.AddListener(OnOnlineLabChoiceB);
		}

		void OnOnlineLabChoiceA(bool isOn)
		{
			if (isOn)
			{
				if (isA)
				{
					isRight = true;
					HideSingleChoice();
				}
				else
				{
					txtOnlineLabTip.gameObject.SetActive(true);
				}
			}
		}

		public bool CheckOnlinLabChoice()
		{
			return isRight;
		}

		void OnOnlineLabChoiceB(bool isOn)
		{
			if (isOn)
			{
				if (!isA)
				{
					isRight = true;
					HideSingleChoice();
				}
				else
				{
					txtOnlineLabTip.gameObject.SetActive(true);
				}
			}
		}

		void SwitchOnlineLabTipContent()
		{
			Vector3 scale = imgRetract.transform.localScale;
			scale.y = -scale.y;
			imgRetract.transform.localScale = scale;
			objOnlinLabTipContent.SetActive(!objOnlinLabTipContent.activeInHierarchy);
		}

		void ShowMainPanel()
		{
			StartCoroutine(DoShowMainPanel());
		}

		IEnumerator DoShowMainPanel()
		{
			StepManager.GetInstance().ClearStep();
			TaskManager.Instance.Exit();
			yield return SceneManager.UnloadSceneAsync(2);
			MainPanel.Instance.Show();
			Hide();
		}


		void HideObjGoBack()
		{
			objGoBack.SetActive(false);
		}

		void ShowObjGoBack()
		{
			objGoBack.gameObject.SetActive(true);
		}

		void SwitchHelp()
		{
			objHelp.SetActive(!objHelp.activeInHierarchy);
		}

		public void NextTask()
		{
			//����Task�����
			if (childIndex >= txtChildList.Count - 1)
			{
				EndTask();
				return;
			}
			txtChildList[childIndex].color = Color.gray;
			childIndex++;
			txtChildList[childIndex].color = Color.white;
			if (!objChildStepFatherList[fatherIndex].GetComponentsInChildren<Text>().Contains(txtChildList[childIndex]))
			{
				togList[fatherIndex].interactable = false;
				togList[fatherIndex].isOn = false;
				(togList[fatherIndex].transform as RectTransform).sizeDelta = new Vector2(184, 75);
				objChildStepFatherList[fatherIndex].SetActive(false);
				fatherIndex++;
				togList[fatherIndex].interactable = true;
				(togList[fatherIndex].transform as RectTransform).sizeDelta = new Vector2(240, 80);

				LayoutRebuilder.MarkLayoutForRebuild(stepRect);
			}
		}

		void EndTask()
		{
			Debug.Log("����Task�������");
		}

		void HideSketch()
		{
			imgSketch.DoShowAnim();
		}

		void SwitchSketch()
		{
			if (imgSketch.isShow)
				imgSketch.DoHideAnim(btnSketch.transform.position);
			else
				imgSketch.DoShowAnim();
		}

		protected override void Start()
		{
			StartCoroutine(HideAsync(0.05f));

			InitListener();
			base.Start();
		}

		protected override void InitState()
		{
			//����Tog
			togList[0].interactable = true;
			togList[0].isOn = true;
			for (int i = 1; i < togList.Count; i++)
			{
				togList[i].interactable = false;
				togList[i].isOn = false;
			}
			fatherIndex = 0;
			//����ChildObj
			objChildStepFatherList[0].SetActive(true);
			for (int i = 1; i < objChildStepFatherList.Count; i++)
				objChildStepFatherList[i].SetActive(false);
			//����ChildText
			txtChildList[0].color = Color.white;
			for (int i = 1; i < txtChildList.Count; i++)
				txtChildList[i].color = Color.gray;
			childIndex = 0;
			//��������
			objGoBack.SetActive(false);
			objHelp.SetActive(false);
			imgSketch.DoHideAnimImmediately(btnSketch.transform.position);
			inputWaxTubeWidth.text = "";
			inputWaxTubeWidth.gameObject.SetActive(false);
		}


		public void ShowSketch(List<Sprite> sprites)
		{
			if (sprites != null)
			{
				for (int i = 0; i < sprites.Count && i < imgSketchs.Count; i++)
				{
					imgSketchs[i].sprite = sprites[i];
				}
			}
			imgSketch.DoShowAnim();
		}


		public void ShowOnlineTip(string newTip)
		{
			objOnlinLabTipHeader.SetActive(true);
			txtOnlineLabTipContent.text = newTip;
		}

		public void HideOnlineTip()
		{
			objOnlinLabTipHeader.SetActive(false);
		}

		public void ShowSingleChoice(string strTitle, string strA, string strB, bool isA, string tip)
		{
			this.isA = isA;
			txtA.text = strA;
			txtB.text = strB;
			txtOnlineLabTip.text = tip;
			txtOnlineLabTip.gameObject.SetActive(false);
			onlineLabChoice.SetActive(true);
		}
		public void HideSingleChoice()
		{
			onlineLabChoice.SetActive(false);
			txtOnlineLabTip.gameObject.SetActive(false);
		}

		public override void Show()
		{
			base.Show();
			StartCoroutine(LoadOnlineLabScene());
		}

		IEnumerator LoadOnlineLabScene()
		{
			yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
			yield return new WaitWhile(() => { return TaskToolManager.Instance == null; });
			TaskManager.Instance.StartTask();
		}
	}
}

