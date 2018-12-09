function initMap(vehicles, pointsOfDelivery) {

    var pointOfInterestIconStyle = new ol.style.Style({
        image: new ol.style.Icon(({
            scale: 0.2,
            anchor: [0.0, 1.0],
            anchorXUnits: 'fraction',
            anchorYUnits: 'fraction',
            src: "/Content/Images/pod-map-marker.png"
        }))
    });

    var vehicleIconStyle = new ol.style.Style({
        image: new ol.style.Icon(({
            scale: 0.2,
            anchor: [0.0, 1.0],
            anchorXUnits: 'fraction',
            anchorYUnits: 'fraction',
            src: "/Content/Images/vehicle-map-marker.png"
        }))
    });

    var iconFeaturesList = [];
    for (var i = 0; i < vehicles.length; i++) {
        iconFeaturesList.push(createIconFeature(vehicles[i], vehicleIconStyle));
    }
    for (var i = 0; i < pointsOfDelivery.length; i++) {
        iconFeaturesList.push(createIconFeature(pointsOfDelivery[i], pointOfInterestIconStyle));
    }

    var vectorSource = new ol.source.Vector({
        features: iconFeaturesList
    });

    var vectorLayer = new ol.layer.Vector({
        source: vectorSource
    });
    
    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            }),
            vectorLayer
        ],
        controls: ol.control.defaults().extend([
            new ol.control.FullScreen(),
            new ol.control.Zoom(),
            new ol.control.Attribution(),
            new ol.control.OverviewMap()
        ]),
        view: new ol.View({
            center: ol.proj.fromLonLat([-73.982, 40.748]),
            zoom: 16
        })
    });

}

function createIconFeature(localisation, iconStyle) {
    var iconFeature = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.fromLonLat(localisation))
    });
    iconFeature.setStyle(iconStyle);
    return iconFeature;
}