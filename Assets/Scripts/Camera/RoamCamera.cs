using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
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
		[SerializeField] CinemachineVirtualCamera fixedCamera = null;
		[SerializeField] CinemachineVirtualCamera lookAtCamera = null;

		Vector3 originPos;
		Vector3 originAngle;
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
				//if (value == false)
				//	BackOrigin();
				isEnable = value;
			}
		}

		public ulong ActionID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public IAction Action { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public bool Paused { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool isRotate = false;

		private void Start()
		{
			//查找组件
			if (rig == null)
				rig = GetComponent<Rigidbody>();
			if (roamCamera == null)
				roamCamera = GetComponent<CinemachineVirtualCamera>();
			//记录初始位置
			originPos = transform.position;
			originAngle = transform.rotation.eulerAngles;


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
		}

		private void BackOrigin()
		{
			if (rig == null)
				rig = GetComponent<Rigidbody>();
			rig.MovePosition(originPos);
			rig.constraints = 0;
			Quaternion quaternion = transform.rotation;
			quaternion.eulerAngles = originAngle;
			rig.MoveRotation(quaternion);
			rig.constraints = RigidbodyConstraints.FreezeRotation;
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

		public void MoveTo(Vector3 targetPos)
		{
			//transform.DOMove(targetPos, 2);

			StartCoroutine(MoveToAsync(targetPos));
		}
		public void MoveToOrigin()
		{
			StopAllCoroutines();
			StartCoroutine(MoveToAsync(originPos));
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

			float firstSpan = Mathf.Min(followTime * 0.33f, 0.1f);
			float secondSpan = followTime - firstSpan;

			var sequence = ActionKit.Sequence()
			.DelayFrame(1)
			.Callback(() =>
			{
				lookAtCamera.Priority = 12;
				roamCamera.Priority = 11;
				fixedCamera.Priority = 10;
			})
			.Delay(followTime, () =>
			{
				lookAtCamera.LookAt = null;
				roamCamera.transform.position = lookAtCamera.transform.position;
				roamCamera.transform.rotation = lookAtCamera.transform.rotation;
			}).DelayFrame(2)
			.Callback(() =>
			{
				roamCamera.Priority = 12;
				lookAtCamera.Priority = 11;
				fixedCamera.Priority = 10;
			})
			.Start(this);
			acList.Add(sequence);
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		public void Deinit()
		{
			throw new NotImplementedException();
		}
	}

}
