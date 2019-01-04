import requests
import json

# Calculated with use of http://project-osrm.org/
def getJsonDataFromOSRM(query):
    webserver = "http://94.245.106.244:5000" # change to http://router.project-osrm.org for demo server (handles 5000 requests/min)
    url = webserver + query
    contents = requests.get(url=url)
    jsonData = contents.json()
    return jsonData

def getRoutingFromListOfPoints(points):
    pointsList = ""
    for p in points[:-1]:
        pointsList += str(p[0]) + ',' + str(p[1]) + ';'
    pointsList += str(points[-1][0]) + ',' + str(points[-1][1])
    query = "/route/v1/driving/" + pointsList + "?steps=true&geometries=polyline&overview=false"
    return getJsonDataFromOSRM(query)

def getRoutingDistanceFromListOfPoints(points):
    return getRoutingFromListOfPoints(points)["routes"][0]["distance"]
        