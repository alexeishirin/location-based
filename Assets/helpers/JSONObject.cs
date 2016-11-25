using System.Collections.Generic;
using System.Collections;
using System;

public class JSONObject {
	public List<JSONProperty> properties = new List<JSONProperty>();

	public void addProperty(JSONProperty property) {
		this.properties.Add (property);
	}

	public string toJSONString () {
		string jsonString = "{";

		foreach(JSONProperty property in this.properties) {
			jsonString += property.toJSONString();
			if (this.properties.IndexOf (property) != this.properties.Count - 1) {
				jsonString += ",";
			}
		}

		jsonString += "}";

		return jsonString;
	}
}