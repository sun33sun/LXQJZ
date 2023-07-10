using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ
{
	public class ParticleManager : BaseManager<ParticleManager>
	{
		static Dictionary<ParticleSystem,IEnumerator> particleDic = new Dictionary<ParticleSystem, IEnumerator>();

		public static List<ParticleSystem> particleList = new List<ParticleSystem>();


		public static void Play(GameObject obj, int time)
		{
			ParticleSystem particle = obj.GetComponent<ParticleSystem>();
			if (particle == null)
				return;
			Stop(obj);
			//ÐÂÐ­³Ì
			IEnumerator newIE = GetInstance().PlayAsync(particle, time);
			particleDic.Add(particle, newIE);
			MonoMgr.GetInstance().StartCoroutine(newIE);
		}

		public static void Stop(GameObject obj)
		{
			ParticleSystem particle = obj.GetComponent<ParticleSystem>();
			if (particle == null)
				return;
			if (particleDic.ContainsKey(particle))
			{
				MonoMgr.GetInstance().StopCoroutine(particleDic[particle]);
				particleDic.Remove(particle);
			}
			particle.Stop();
		}

		IEnumerator PlayAsync(ParticleSystem particle, int time)
		{
			particle.Play();
			yield return new WaitForSeconds(time);
			EventCenter.GetInstance().EventTrigger(particle.name + "ParticleEnd" + time);
			particle.Stop();
			particleDic.Remove(particle);
		}
	}
}
