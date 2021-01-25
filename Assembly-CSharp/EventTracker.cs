using System;
using System.Collections.Generic;
using System.Text;
using JsonFx.Json;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class EventTracker
{
	// Token: 0x060006EF RID: 1775 RVA: 0x00006255 File Offset: 0x00004455
	public static WWW Track(Guid guid, TrackEventType eventType, object obj)
	{
		return EventTracker.Track(Convert.ToBase64String(guid.ToByteArray()), eventType, obj);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00047EEC File Offset: 0x000460EC
	public static WWW Track(string id, TrackEventType eventType, object obj)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		if (obj == null)
		{
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("event", eventType);
		Dictionary<string, object> dictionary2 = dictionary;
		string text = "properties";
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		dictionary3.Add("token", "5f571dcd0b966c7a4a7053198e0db942");
		dictionary3.Add("distinct_id", id);
		dictionary3.Add("data", obj);
		dictionary2.Add(text, dictionary3);
		Dictionary<string, object> dictionary4 = dictionary;
		string text2 = JsonWriter.Serialize(dictionary4);
		byte[] bytes = Encoding.UTF8.GetBytes(text2);
		string text3 = Convert.ToBase64String(bytes);
		return new WWW("http://api.mixpanel.com/track/?data=" + text3);
	}

	// Token: 0x0400075C RID: 1884
	private const string uri = "http://api.mixpanel.com/track/?data=";

	// Token: 0x0400075D RID: 1885
	private const string token = "5f571dcd0b966c7a4a7053198e0db942";
}
