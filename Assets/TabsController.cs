using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TabsController : MonoBehaviour, IEndDragHandler, IBeginDragHandler {
	public UnityEngine.UI.Scrollbar horizontalScroll;
	public GameObject content;
	public GameObject headersContainer;
	public GameObject underline;

	private int currentActivePanelIndex = -1;
	public float targetScrollBarPosition = -1.0f;
	private float startScrollBarPosition = -1.0f;

	public float targetUnderlinePositionX = 0.0f;
	public float startUnderlinePositionX = 0.0f;
	public float targetUnderlineWidth = 0.0f;
	public float startUnderlineWidth = 0.0f;

	public float SWIPE_DURATION = 0.2f;
	private float animationSeconds = 0.0f;

	public void Start () {
		horizontalScroll.value = 0.0f;
		this.getTabHeaders () [0].GetComponent<TabHeaderController> ().setActive (true);
		targetUnderlinePositionX = startUnderlinePositionX = underline.transform.position.x;
		foreach (RectTransform tabHeader in this.getTabHeaders()) {
			tabHeader.GetComponent<TabHeaderController> ().SWIPE_DURATION = this.SWIPE_DURATION;
		}
	}

	public void Update() {
		if (targetScrollBarPosition != -1 && this.animationSeconds < SWIPE_DURATION) {
			animationSeconds += Time.deltaTime;
			float t = animationSeconds / SWIPE_DURATION;
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			horizontalScroll.value = Mathf.Lerp (startScrollBarPosition, targetScrollBarPosition, t);
			Vector3 newUnderlinePosition = underline.transform.position;
			newUnderlinePosition.x = Mathf.Lerp (startUnderlinePositionX, targetUnderlinePositionX, t);
			underline.transform.position = newUnderlinePosition;
			float newUnderlineWidth = Mathf.Lerp (startUnderlineWidth, targetUnderlineWidth, t);
			underline.GetComponent<RectTransform> ().sizeDelta = new Vector2 (newUnderlineWidth, underline.GetComponent<RectTransform> ().sizeDelta.y);
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (eventData.pressPosition.x < eventData.position.x) {
			this.swipeRight ();
		} else {
			this.swipeLeft ();
		}
	}

	public void OnBeginDrag (PointerEventData eventData) {
		this.targetScrollBarPosition = -1.0f;
		this.currentActivePanelIndex = this.getClosestToCenterPanelIndex ();
	}

	public void swipeLeft () {
		if (this.currentActivePanelIndex != this.getPanelCount () - 1) {
			this.setActivePage (this.currentActivePanelIndex + 1);
		}
	}

	public void swipeRight () {
		if (this.currentActivePanelIndex != 0) {
			this.setActivePage(this.currentActivePanelIndex - 1);
		}
	}

	public void setActivePage(int newActivePageIndex) {
		if (this.currentActivePanelIndex != newActivePageIndex) {
			this.animationSeconds = 0.0f;
			this.startScrollBarPosition = horizontalScroll.value;
			this.targetScrollBarPosition = newActivePageIndex / (this.getPanelCount () - 1.0f);
			this.setTabHeadersActive (newActivePageIndex);
		}
	}

	public void setTabHeadersActive(int newActiveIndex) {
		int dotIndex = 0;
		foreach (RectTransform tabHeader in this.getTabHeaders()) {
			tabHeader.GetComponent<TabHeaderController> ().setActive (dotIndex == newActiveIndex);
			if (dotIndex == newActiveIndex) {
				this.startUnderlinePositionX = this.underline.transform.position.x;
				this.targetUnderlinePositionX = tabHeader.transform.position.x;
				this.startUnderlineWidth = this.underline.GetComponent<RectTransform>().rect.width;
				this.targetUnderlineWidth = tabHeader.GetComponent<RectTransform>().rect.width;
			}
			dotIndex++;
		}
	}


	public int getPanelCount() {
		RectTransform[] childrenPanels = content.GetComponentsInChildren<RectTransform> ();
		int numberOfPanels = 0;
		foreach (RectTransform rectTransform in childrenPanels) {
			if (rectTransform.parent == content.transform) {
				numberOfPanels++;
			}
		}

		return numberOfPanels;
	}

	public List<RectTransform> getTabHeaders() {
		RectTransform[] tabHeaders = headersContainer.GetComponentsInChildren<RectTransform> ();
		List<RectTransform> headersList = new List<RectTransform>();
		foreach (RectTransform button in tabHeaders) {
			if (button.transform.parent == headersContainer.transform) {
				headersList.Add (button);
			}
		}

		return headersList;
	}

	public int getClosestToCenterPanelIndex() {
		int panelCount = this.getPanelCount ();
		float currentScrollValue = horizontalScroll.value;
		int closestToCenterPanelIndex = -1;
		float minDifference = 2.0f;

		for (int panelIndex = 0; panelIndex < panelCount; panelIndex++) {
			float panelScrollPosition = panelIndex * 1.0f / (panelCount - 1);
			if (closestToCenterPanelIndex == -1) {
				minDifference = Mathf.Abs (panelScrollPosition - currentScrollValue);
				closestToCenterPanelIndex = panelIndex;
				continue;
			}

			if (Mathf.Abs (panelScrollPosition - currentScrollValue) < minDifference) {
				minDifference = Mathf.Abs (panelScrollPosition - currentScrollValue);
				closestToCenterPanelIndex = panelIndex;
			}
		}

		return closestToCenterPanelIndex;
	}
}
