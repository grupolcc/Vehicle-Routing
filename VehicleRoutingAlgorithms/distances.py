import requests
import json
from pprint import pprint


# Set default distance calculator here
def calculateDistance(position_1, position_2):
    return actualDistance(position_1, position_2)

def manhattanDistance(position_1, position_2):
    return abs(position_1[0] - position_2[0]) + abs(position_1[1] - position_2[1])

# Calculated with use of http://project-osrm.org/
def actualDistance(position_1, position_2):
    webserver = "http://94.245.106.244:5000" # change to http://router.project-osrm.org for demo server (handles 5000 requests/min)
    url = webserver + "/route/v1/driving/" + str(position_1[0]) + ',' + str(position_1[1]) + ';' + str(position_2[0]) + ',' + str(position_2[1]) + "?steps=true&geometries=polyline&overview=false"
    contents = requests.get(url=url)
    jsonData = contents.json()
    distance = jsonData["routes"][0]["distance"]

    return distance

