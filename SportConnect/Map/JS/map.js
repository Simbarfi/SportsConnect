var myMap = L.map('mapid').setView([44.0262, -88.5517], 15);
L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
	attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
	maxZoom: 18,
	id: 'mapbox/streets-v11',
	accessToken: 'pk.eyJ1IjoiYWJlbHQiLCJhIjoiY2t2aDVodHpzMmZzZjMxczFjbGgxZXM3dSJ9.kSVKQeL9NP2AICuS_dUAzA'
}).addTo(myMap);

var popup = L.popup();

var MapMarker = L.Marker.extend({
	initialize: function (latlng) {
		L.Marker.prototype.initialize.call(this, latlng);
	},
	click: function () {
		L.Marker.prototype.click.call(this, latlng);
		window.external.ViewEvent();
	}
});

function onMapClick(e) {
	var tempMarker = L.marker(e.latlng).addTo(myMap);
	tempMarker.on('click', function () { window.external.ViewEvent() });
	//var tm = new MapMarker(e.latlng);
	window.external.CreateEvent(e.latlng);
	tempMarker.removeFrom(myMap);
}


myMap.on('click', onMapClick); 



