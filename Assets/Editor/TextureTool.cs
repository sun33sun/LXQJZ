using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LXQJZ
{
	public class TextureTool : MonoBehaviour
	{
  //      static string path = "D:\\WorkSpace\\LXQJZ\\Assets\\Resources\\image\\Ring";
  //      public static void Play()
		//{
            
		//}

  //      public static Texture2D ClipBlank(Texture2D orgin)
  //      {
  //          try
  //          {
  //              var left = 0;
  //              var top = 0;
  //              var right = orgin.width;
  //              var botton = orgin.height;

  //              // ���
  //              for (var i = 0; i < orgin.width; i++)
  //              {
  //                  var find = false;
  //                  for (var j = 0; j < orgin.height; j++)
  //                  {
  //                      var color = orgin.GetPixel(i, j);
  //                      if (Math.Abs(color.a) > 1e-6)
  //                      {
  //                          find = true;
  //                          break;
  //                      }
  //                  }
  //                  if (find)
  //                  {
  //                      left = i;
  //                      break;
  //                  }
  //              }

  //              // �Ҳ�
  //              for (var i = orgin.width - 1; i >= 0; i--)
  //              {
  //                  var find = false;
  //                  for (var j = 0; j < orgin.height; j++)
  //                  {
  //                      var color = orgin.GetPixel(i, j);
  //                      if (Mathf.Abs(color.a) > 1e-6)
  //                      {
  //                          find = true;
  //                          break;
  //                      }
  //                  }
  //                  if (find)
  //                  {
  //                      right = i;
  //                      break;
  //                  }
  //              }

  //              // �ϲ�
  //              for (var j = 0; j < orgin.height; j++)
  //              {
  //                  var find = false;
  //                  for (var i = 0; i < orgin.width; i++)
  //                  {
  //                      var color = orgin.GetPixel(i, j);
  //                      if (Mathf.Abs(color.a) > 1e-6)
  //                      {
  //                          find = true;
  //                          break;
  //                      }
  //                  }
  //                  if (find)
  //                  {
  //                      top = j;
  //                      break;
  //                  }
  //              }

  //              // �²�
  //              for (var j = orgin.height - 1; j >= 0; j--)
  //              {
  //                  var find = false;
  //                  for (var i = 0; i < orgin.width; i++)
  //                  {
  //                      var color = orgin.GetPixel(i, j);
  //                      if (Mathf.Abs(color.a) > 1e-6)
  //                      {
  //                          find = true;
  //                          break;
  //                      }
  //                  }
  //                  if (find)
  //                  {
  //                      botton = j;
  //                      break;
  //                  }
  //              }

  //              // ����������
  //              var width = right - left;
  //              var height = botton - top;

  //              var result = new Texture2D(width, height, TextureFormat.ARGB32, false);
  //              result.alphaIsTransparency = true;

  //              // ������Ч��ɫ����
  //              var colors = orgin.GetPixels(left, top, width, height);
  //              result.SetPixels(0, 0, width, height, colors);

  //              result.Apply();
  //              return result;
  //          }
  //          catch (Exception e)
  //          {
  //              return null;
  //          }
  //      }
    }
}
