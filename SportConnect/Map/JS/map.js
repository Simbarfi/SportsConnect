//window.chrome.webview.postMessage()
//window.chrome.webview.addEventListener(\'message\', event => alert(event.data));"
var myMap;
var wv = window.chrome.webview;
var tempMarker;

function getMarker(latlng) {
	var mark = L.marker(latlng);
	return mark;
}

function onMapClick(e) {
	tempMarker = getMarker(e.latlng);
	tempMarker.addTo(myMap);
	wv.postMessage(`CreateEvent@${e.latlng}`);
}

function CreateEvent(response) {
	if (response.length !== 0) {
		response = JSON.parse(response);
		var popupMessage = '<p>' + response.Name + '</p>' +
			'<p>' + response.Sport + '</p>' +
			'<p>' + response.Start + '  ' + response.End + '</p>' +
			'<p>' + response.MaxPlayers + '</p>' +
			'<p>' + response.SkillLevel + '</p>' +
			'<p>' + response.Location + '</p>';
		tempMarker.bindPopup(popupMessage);
	}
	else {
		tempMarker.removeFrom(myMap);
	}
}

function setup() {
	myMap = L.map('mapid').setView([44.0262, -88.5517], 15);
	L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
		attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
		maxZoom: 18,
		id: 'mapbox/streets-v11',
		accessToken: 'pk.eyJ1IjoiYWJlbHQiLCJhIjoiY2t2aDVodHpzMmZzZjMxczFjbGgxZXM3dSJ9.kSVKQeL9NP2AICuS_dUAzA'
	}).addTo(myMap);

	myMap.on('click', onMapClick);
}

function attendEvent(e) {
	wv.postMessage(`AttendEvent@${e}`);
}


setup();

wv.addEventListener('message', event => CreateEvent(event.data));






