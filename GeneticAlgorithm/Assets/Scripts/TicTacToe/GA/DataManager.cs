using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TicTacToe.GA
{
	public class DataManager
	{
		public static bool WriteData<T>(T target, string path)
		{
			var text = Serialize(target);
			try 
			{
				using (var writer = new StreamWriter(Application.dataPath + path,false)){
					writer.Write(text);
					writer.Flush();
					writer.Close();
				}
			} 
			catch (Exception e) 
			{
				Debug.Log(e.Message);
				return false;
			}
			return true;
		}

		/// <summary>
		/// 行列を読み込む
		/// </summary>
		/// <param name="path">Assets以下のパスを指定する</param>
		/// <returns></returns>
		public static T ReadData<T>(string path)
		{
			//ストリームリーダーsrに読み込む
			var strStream = "";
			try 
			{
				using (var sr = new StreamReader(Application.dataPath + path)){
					//ストリームリーダーをstringに変換
					strStream = sr.ReadToEnd();
					sr.Close();
				}
			} 
			catch (Exception e) 
			{
				Debug.Log(e.Message);
			}

			return Deserialize<T>(strStream);
		}
		
		private static string Serialize<T> (T obj){
			var binaryFormatter = new BinaryFormatter ();
			var memoryStream    = new MemoryStream ();
			binaryFormatter.Serialize (memoryStream , obj);
			return Convert.ToBase64String (memoryStream   .GetBuffer ());
		}

		private static T Deserialize<T> (string str){
			var binaryFormatter = new BinaryFormatter ();
			var memoryStream    = new MemoryStream (Convert.FromBase64String (str));
			return (T)binaryFormatter.Deserialize (memoryStream);
		}

	}
}
