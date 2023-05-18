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
	public class RoamCamera : SingletonMono<RoamCamera>
	{
		[SerializeField] float horizontalSpeed = 3;
		[SerializeField] float verticalSpeed = 2;
		[SerializeField] float rotateSpeed = 3;
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

		bool isRotate = false;

		private void Start()
		{
			//�������
			if (rig == null)
				rig = GetComponent<Rigidbody>();
			if (roamCamera == null)
				roamCamera = GetComponent<CinemachineVirtualCamera>();
			//��¼��ʼλ��
			originPos = transform.position;
			originAngle = transform.rotation.eulerAngles;


			InputMgr.GetInstance().StartOrEndCheck(true);
			EventCenter.GetInstance().AddEventListener(KeyCode.W + "����", OnWState);
			EventCenter.GetInstance().AddEventListener(KeyCode.A + "����", OnAState);
			EventCenter.GetInstance().AddEventListener(KeyCode.S + "����", OnSState);
			EventCenter.GetInstance().AddEventListener(KeyCode.D + "����", OnDState);

			EventCenter.GetInstance().AddEventListener(KeyCode.W + "̧��", OnWUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.A + "̧��", OnAUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.S + "̧��", OnSUp);
			EventCenter.GetInstance().AddEventListener(KeyCode.D + "̧��", OnDUp);

			EventCenter.GetInstance().AddEventListener("����Ҽ�����", OnMouseRightDown);
			EventCenter.GetInstance().AddEventListener("����Ҽ�̧��", OnMouseRightUp);
			EventCenter.GetInstance().AddEventListener<Vector2>("��껬��", OnMouseSliding);

			EventCenter.GetInstance().AddEventListener(KeyCode.LeftControl + "����", OnLeftControlState);

			EventCenter.GetInstance().AddEventListener(KeyCode.Space + "����", OnSpaceState);
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

		#region ������Ӧ�¼�
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
			if (!IsEnable)
				return;
			if (!isRotate)
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
			lookAtCamera.LookAt = target;

			float firstSpan = Mathf.Min(followTime * 0.33f, 0.1f);
			float secondSpan = followTime - firstSpan;

			ActionKit.Sequence()
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
		}
	}

}
