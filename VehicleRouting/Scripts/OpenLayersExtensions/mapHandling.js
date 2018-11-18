function initMap(listOfPoints) { //TODO: handle a list of Points
    var iconFeature = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.fromLonLat([18.618, 54.373]))
    });

    var iconStyle = new ol.style.Style({
        image: new ol.style.Icon(({
            scale: 0.1,
            anchor: [0.5, 1.0],
            anchorXUnits: 'fraction',
            anchorYUnits: 'fraction',
            src: "/Content/Images/map-marker.png"
        }))
    });

    iconFeature.setStyle(iconStyle);

    var vectorSource = new ol.source.Vector({
        features: [iconFeature]
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
            center: ol.proj.fromLonLat([18.618, 54.373]),
            zoom: 16
        })
    });
}