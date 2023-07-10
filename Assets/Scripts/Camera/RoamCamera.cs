using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using QFramework;


namespace LXQJZ
{
	public class RoamCamera : SingletonMono<RoamCamera>, IActionController
	{
		List<IActionController> acList = new List<IActionController>();

		[SerializeField] float horizontalSpeed = 3;
		[SerializeField] float verticalSpeed = 2;
		[SerializeField] float rotateSpeed = 3;
		[SerializeField] float mouseScrollWheelSpeed = 1;
		[SerializeField] [Range(0, 1)] float drag = 0.8f;

		Rigidbody rig = null;
		CinemachineVirtualCamera roamCamera = null;
		[SerializeField] CinemachineVirtualCamera lookAtCamera = null;
		public CinemachineVirtualCamera lookAtCamera2 = null;
		[SerializeField] CinemachineVirtualCamera followCamera = null;

		Vector3 originPos;
		Vector3 originAngle;
		float originFieldOfView;
		Vector3 nowPos;
		bool isEnable = true;
		public bool IsEnable
		{
			get
			{
				return isEnable && !AnimManager.IsPlaying;
			}
			set
			{
				isEnable = value;
			}
		}

		public ulong ActionID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public IAction Action { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool Paused { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool isRotate = false;

		protected override void Awake()
		{
			needDestroy = true;
			base.Awake();
		}

		private void Start()
		{
			//查找组件
			rig = GetComponent<Rigidbody>();
			roamCamera = GetComponent<CinemachineVirtualCamera>();
			//记录初始位置
			originPos = transform.position;
			originAngle = transform.rotation.eulerAngles;
			originFieldOfView = roamCamera.m_Lens.FieldOfView;


			InputMgr.GetInstance().StartOrEndCheck(true);
			EventCenter.GetInstance().AddEventListener(KeyCode.W + "保持", OnWState);
			EventCenter.GetInstance().AddEventListener(KeyCode.A + "保持", OnAState);
			EventCenter.GetInstance().AddEventListener(KeyCode.S + "保持", OnSState);
			EventCenter.GetInstance().AddEventListener(KeyCode.D + "保持", OnDState);

			EventCenter.GetInstance().AddEventListener(KeyCode.W + "抬起", OnWUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.A + "抬起", OnAUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.S + "抬起", OnSUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.D + "抬起", OnDUp);

			EventCenter.GetInstance().AddEventListener("鼠标右键按下", OnMouseRightDown);
			EventCenter.GetInstance().AddEventListener("鼠标右键抬起", OnMouseRightUp);
			EventCenter.GetInstance().AddEventListener<Vector2>("鼠标滑动", OnMouseSliding);

			EventCenter.GetInstance().AddEventListener(KeyCode.LeftControl + "保持", OnLeftControlState);

			EventCenter.GetInstance().AddEventListener(KeyCode.Space + "保持", OnSpaceState);

			EventCenter.GetInstance().AddEventListener<float>("鼠标滚轮", OnMouseScrollWheel);

			ActionKit.Delay(1, () => { gameObject.SetActive(false); });
		}

		#region 按键响应事件
		private void OnWState()
		{
			if (!IsEnable)
				return;
			rig.velocity = transform.forward * horizontalSpeed;
		}

		private void OnAState()
		{
			if (!IsEnable)
				return;
			rig.velocity = transform.right * -horizontalSpeed;
		}

		private void OnSState()
		{
			if (!IsEnable)
				return;
			rig.velocity = transform.forward * -horizontalSpeed;
		}

		private void OnDState()
		{
			if (!IsEnable)
				return;
			rig.velocity = transform.right * horizontalSpeed;
		}

		private void OnWUp()
		{
			if (!IsEnable)
				return;
			rig.velocity -= transform.forward * horizontalSpeed * drag;
		}

		private void OnAUp()
		{
			if (!IsEnable)
				return;
			rig.velocity -= transform.right * -horizontalSpeed * drag;
		}

		private void OnSUp()
		{
			if (!IsEnable)
				return;
			rig.velocity -= transform.forward * -horizontalSpeed * drag;
		}

		private void OnDUp()
		{
			if (!IsEnable)
				return;
			rig.velocity -= transform.right * horizontalSpeed * drag;
		}

		private void OnMouseRightDown()
		{
			if (!IsEnable)
				return;
			isRotate = true;
		}
		private void OnMouseRightUp()
		{
			if (!IsEnable)
				return;
			isRotate = false;
		}
		private void OnMouseSliding(Vector2 vec2)
		{
			if (!isRotate || lookAtCamera.Priority > roamCamera.Priority)
				return;
			transform.RotateAround(transform.position, Vector3.up, vec2.x * rotateSpeed);
		}
		private void OnLeftControlState()
		{
			if (!IsEnable)
				return;
			nowPos = transform.localPosition;
			nowPos.y -= verticalSpeed * Time.fixedDeltaTime;
			transform.localPosition = nowPos;
		}
		private void OnSpaceState()
		{
			if (!IsEnable)
				return;
			nowPos = transform.localPosition;
			nowPos.y += verticalSpeed * Time.fixedDeltaTime;
			transform.localPosition = nowPos;
		}
		private void OnMouseScrollWheel(float distance)
		{
			roamCamera.m_Lens.FieldOfView += distance * mouseScrollWheelSpeed;
			if (roamCamera.m_Lens.FieldOfView < 1)
				roamCamera.m_Lens.FieldOfView = 1;
			else if (roamCamera.m_Lens.FieldOfView > 90)
				roamCamera.m_Lens.FieldOfView = 90;
		}
		#endregion

		public void BackToOrigin()
		{
			for (int i = acList.Count - 1; i > -1; i--)
			{
				acList[i].Deinit();
				acList.RemoveAt(i);
			}
			StopAllCoroutines();
			StartCoroutine(MoveToAsync(originPos));
			roamCamera.m_Lens.FieldOfView = originFieldOfView;

			roamCamera.Priority = 12;
			lookAtCamera.Priority = 11;
			lookAtCamera.LookAt = null;
			followCamera.Priority = 10;
			followCamera.Follow = null;
		}

		IEnumerator MoveToAsync(Vector3 targetPos)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (Vector3.Distance(targetPos, transform.position) > 0.1f)
			{
				Vector3 dir = Vector3.Normalize(targetPos - transform.position) * Time.deltaTime * 10;
				rig.MovePosition(transform.position + dir);
				yield return wait;
			}
		}


		public void LookAt(Transform target, float followTime)
		{
			if(acList.Count > 0)
			{
				for (int i = 0; i < acList.Count; i++)
					acList[i].Deinit();
			}
			lookAtCamera.LookAt = target;

			var sequence = ActionKit.Sequence()
			.DelayFrame(1)
			.Callback(() =>
			{
				lookAtCamera.Priority = 12;
				roamCamera.Priority = 11;
				followCamera.Priority = 10;
			})
			.Delay(followTime, () =>
			{
				lookAtCamera.LookAt = null;
				roamCamera.transform.position = lookAtCamera.transform.position;
				roamCamera.transform.rotation = lookAtCamera.transform.rotation;
				roamCamera.Priority = 12;
				lookAtCamera.Priority = 11;
				followCamera.Priority = 10;
				roamCamera.m_Lens.FieldOfView = lookAtCamera.m_Lens.FieldOfView;
			})
			.Start(this);
			acList.Add(sequence);
		}
		
		public void LookAt(Transform target)
		{
			if(target != null)
			{
				lookAtCamera.LookAt = target;
				lookAtCamera.Priority = 12;
				roamCamera.Priority = 11;
				followCamera.Priority = 10;
			}
			else
			{
				lookAtCamera.LookAt = null;
				roamCamera.Priority = 12;
				lookAtCamera.Priority = 11;
				followCamera.Priority = 10;
			}
		}

		public void LookAt2(Transform target)
		{
			if (target != null)
			{
				lookAtCamera2.LookAt = target;
				lookAtCamera2.Priority = 13;
			}
			else
			{
				lookAtCamera2.LookAt = null;
				lookAtCamera2.Priority = 10;
			}
		}

		public void Follow(Transform target)
		{
			if (target != null)
			{
				followCamera.Follow = target;
				followCamera.Priority = 12;
				roamCamera.Priority = 11;
				lookAtCamera.Priority = 10;
			}
			else
			{
				followCamera.Follow = null;
				roamCamera.Priority = 12;
				followCamera.Priority = 11;
				lookAtCamera.Priority = 10;
			}
		}


		public void Reset()
		{
		}

		public void Deinit()
		{
			throw new NotImplementedException();
		}

		private void OnDestroy()
		{
			EventCenter.GetInstance().RemoveEventListener(KeyCode.W + "保持", OnWState);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.A + "保持", OnAState);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.S + "保持", OnSState);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.D + "保持", OnDState);
									 
			EventCenter.GetInstance().RemoveEventListener(KeyCode.W + "抬起", OnWUp);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.A + "抬起", OnAUp);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.S + "抬起", OnSUp);
			EventCenter.GetInstance().RemoveEventListener(KeyCode.D + "抬起", OnDUp);
									  
			EventCenter.GetInstance().RemoveEventListener("鼠标右键按下", OnMouseRightDown);
			EventCenter.GetInstance().RemoveEventListener("鼠标右键抬起", OnMouseRightUp);
			EventCenter.GetInstance().RemoveEventListener<Vector2>("鼠标滑动", OnMouseSliding);
									  
			EventCenter.GetInstance().RemoveEventListener(KeyCode.LeftControl + "保持", OnLeftControlState);
									  
			EventCenter.GetInstance().RemoveEventListener(KeyCode.Space + "保持", OnSpaceState);

			EventCenter.GetInstance().RemoveEventListener<float>("鼠标滚轮", OnMouseScrollWheel);
		}
	}

}
