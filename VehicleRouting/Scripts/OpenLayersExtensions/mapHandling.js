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
            src: "/Content/Images/Car1.png"
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
    
    map = new ol.Map({
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
            zoom: 15
        })
    });
}

function createIconFeature(localization, iconStyle) {
    var iconFeature = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.fromLonLat(localization))
    });
    iconFeature.setStyle(iconStyle);
    return iconFeature;
}

function showResults(vehicles, pointsOfDelivery, results) {
    initMap(vehicles, pointsOfDelivery);
    layers = {};
    for (var key in results) {
        if (results.hasOwnProperty(key)) {
            var randomColor = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
            var points = getIntermediatePoints(results[key]);
            var lineLayer = getLineLayer(points, randomColor);
            map.addLayer(lineLayer);
            layers[key] = lineLayer;
        }
    }
}

function getAllLayers() {
    for (var key in layers) {
        if (layers.hasOwnProperty(key)) {
            map.removeLayer(layers[key]);
            map.addLayer(layers[key]);
        }
    }
}

function labelOnMap(label, coords) {
    var style = new ol.style.Style({
        text: new ol.style.Text({
            text: label,
            scale: 1.3,
            fill: new ol.style.Fill({
                color: '#000000'
            }),
            stroke: new ol.style.Stroke({
                color: '#FFFF99',
                width: 3.5
            })
        })
    });

    var movedCoords = [coords[0] - 0.0002, coords[1] + 0.0002];

    var iconFeature = createIconFeature(movedCoords, style);

    var source = new ol.source.Vector({});
    source.addFeature(iconFeature);

    var vector = new ol.layer.Vector({
        source: source
    });
    map.addLayer(vector);
}

function labelVehicle(vehicleID, coords) {
    labelOnMap('S' + vehicleID, coords);
}

function labelPoint(pointID, coords) {
    labelOnMap('P' + pointID, coords);
}

function getSeparateLayer(vehicleID) {
    for (var key in layers) {
        if (layers.hasOwnProperty(key)) {
            map.removeLayer(layers[key]);
        }
    }
    map.addLayer(layers[vehicleID]);
}

function getPointsBetweenTwoPoints(pointA, pointB) {
    var pts = [];

    var xmlHttp = new XMLHttpRequest();
    var server = "http://94.245.106.244:5000"; //change to http://router.project-osrm.org for demo server (handles 5000 requests/min)
    xmlHttp.open("GET", server + "/route/v1/driving/" + pointA[0] + ',' + pointA[1] + ';' + pointB[0] + ',' + pointB[1] + "?steps=true&geometries=polyline&overview=false", false);
    xmlHttp.send(null);
    var jsonResponse = JSON.parse(xmlHttp.responseText);

    var points = jsonResponse.routes[0].legs[0].steps;

    for (var i = 0; i < points.length; i++) {
        for (var j = 0; j < points[i].intersections.length; j++) {
            pts.push(points[i].intersections[j].location);
        }
    }

    return pts;
}

function getIntermediatePoints(points) {
    var pts = [];
    for (var i = 0; i < points.length; i++) {
        pts.push(points[i]);
        if (i + 1 < points.length) {          
            pts.push.apply(pts, getPointsBetweenTwoPoints(points[i], points[i + 1]));
        } 
    }
    return pts;
}

function getLineLayer(points, col) {
    for (var i = 0; i < points.length; i++) {
        points[i] = ol.proj.transform(points[i], 'EPSG:4326', 'EPSG:3857');
    }

    var featureLine = new ol.Feature({
        geometry: new ol.geom.LineString(points)
    });

    var vectorLine = new ol.source.Vector({});
    vectorLine.addFeature(featureLine);

    var vectorLineLayer = new ol.layer.Vector({
        source: vectorLine,
        style: new ol.style.Style({
            fill: new ol.style.Fill({ color: col, weight: 6 }),
            stroke: new ol.style.Stroke({ color: col, width: 4 })
        })
    });

    return vectorLineLayer;
}