var myMap = L.map('mapid').setView([44.0262, -88.5517], 15);
L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
	attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
	maxZoom: 18,
	id: 'mapbox/streets-v11',
	accessToken: 'pk.eyJ1IjoiYWJlbHQiLCJhIjoiY2t2aDVodHpzMmZzZjMxczFjbGgxZXM3dSJ9.kSVKQeL9NP2AICuS_dUAzA'
}).addTo(myMap);

var popup = L.popup();

function onMapClick(e) {
    window.external.CreateEvent(e.latlng);
}

myMap.on('click', onMapClick);

var q = L.marker([44.0262, -88.5517]).addTo(myMap);
var p = L.marker([44.0262, -88.5520]).addTo(myMap);

