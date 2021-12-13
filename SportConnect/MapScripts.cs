
namespace SportConnect
{
    public static class MapScripts
    {

        public static readonly string PreventRightClickMenu =
            "window.addEventListener('contextmenu', window => {window.preventDefault();});";

        public static readonly string RemoveMarkers =
            "myMap.getPane('markerPane').replaceChildren();";

        public static readonly string RemoveShadows =
            "myMap.getPane('shadowPane').replaceChildren();";

        public static string AddEventToMap(Event newEvent, string hostName)
        {
            return $"var cm = L.marker([{newEvent.Latitude},{newEvent.Longitude}]); " +
                $"var pop = '<div><h1>{newEvent.Name}</h1><div>Hosted by: {hostName}</div>" +
                $"<div>Location: {newEvent.Location}</div><div>Sport: {newEvent.Sport}</div>" +
                $"<div>Start time: {newEvent.Start}</div><div>End time: {newEvent.End}</div>" +
                $"<div>Skill: {newEvent.SkillLevel}</div>" +
                $"<div>Looking for {newEvent.MaxPlayers} players</div></div>';" +
                $"pop += ' <button name=\"{newEvent.Id}\" onclick=\"attendEvent(name)\">Join</button>';" +
                "cm.bindPopup(pop);" +
                "cm.addTo(myMap);";
        }

        public static string ResizeMap(double height, double width)
        {
            return $"document.getElementById('mapid').style.height = '{height}px';" +
                $"document.getElementById('mapid').style.width = '{width}px';" +
                "myMap.invalidateSize(15);";
        }

    }
}
