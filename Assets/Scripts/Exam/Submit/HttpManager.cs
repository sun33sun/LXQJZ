using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using LXQJZ.Exam;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.IO;

namespace LXQJZ.Exam
{
	public class HttpManager : BaseManager<HttpManager>
	{
		public bool IsBusy = false;

		public string url = null;

		public HttpManager()
		{
			MonoMgr.GetInstance().StartCoroutine(GetHttp());
		}

		IEnumerator GetHttp()
		{
			UnityWebRequest request = UnityWebRequest.Get(ProjectSettings.HTTP_TXT);
			yield return request.SendWebRequest();
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogWarning("获取配置信息失败：" + request.error);
				yield break;
			}
			url = request.downloadHandler.text;
			Debug.Log(url);
		}


		public void Post(SubmitData submitData)
		{
			if (!IsBusy)
			{
				IsBusy = true;
				string submitJson = JsonConvert.SerializeObject(submitData);
				File.WriteAllText(Application.streamingAssetsPath + "/SubmitData.json", submitJson);
				//byte[] submitBytes = GetBytes(submitJson);
				//MonoMgr.GetInstance().StartCoroutine(Post(submitBytes));
			}
			else
			{
				Debug.LogWarning("正在向后端传输数据，请稍后");
			}
		}

		byte[] GetBytes(string submitJson)
		{
			Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
			submitJson = reg.Replace(submitJson, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
			Debug.Log(submitJson);
			byte[] submitBytes = Encoding.UTF8.GetBytes(submitJson);//把json格式的字符串转化成数组
			System.Text.Encoding gb2312;
			gb2312 = System.Text.Encoding.GetEncoding("UTF-8");
			string CnStr = gb2312.GetString(submitBytes);
			Debug.Log(CnStr);
			return submitBytes;
		}

		IEnumerator Post(byte[] postBytes)
		{
			yield return new WaitUntil(() => { return url != null; });
			UnityWebRequest request = new UnityWebRequest(url, "POST");//method传输方式，默认为Get
			request.uploadHandler = new UploadHandlerRaw(postBytes);//实例化上传缓存器
			request.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
			request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");//更改内容类型
			//request.SetRequestHeader("Content-Type", "Access-Control-Allow-Origin");//更改内容类型
			yield return request.SendWebRequest();//发送请求
			Debug.Log("Status Code: " + request.responseCode);//获得返回值
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogWarning(request.error);
			}
			IsBusy = false;
			if (request.responseCode == 200)//检验是否成功
			{
				string text = request.downloadHandler.text;//打印获得值
				Debug.Log(text);
			}
		}
	}
}
