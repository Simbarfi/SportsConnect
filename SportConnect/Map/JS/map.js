var myMap;

function getMarker(latlng) {
	var mark = L.marker(latlng);
	mark.on('click', function () { window.external.ViewEvent() });
	return mark;
}

function onMapClick(e) {
	var tempMarker = getMarker(e.latlng);
	tempMarker.addTo(myMap);
	//var tempMarker = L.marker(e.latlng).addTo(myMap);
	//tempMarker.on('click', function () { window.external.ViewEvent() });
	var res = window.external.CreateEvent(e.latlng);
	if(!res)
		tempMarker.removeFrom(myMap);
}

function CreateEvent() {

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
setup();




