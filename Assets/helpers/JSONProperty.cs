public class JSONProperty {
	public string key;
	public object value;

	public JSONProperty (string key, object value) {
		this.key = key;
		this.value = value;
	}

	public string toJSONString () {
		return "\"" + this.key + "\":\"" + this.value.ToString() + "\"";
	}
}