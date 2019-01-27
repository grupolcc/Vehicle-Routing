import requests
from custom_exceptions import *


# Calculated with use of http://project-osrm.org/
def get_json_data_from_osrm(query):
    webserver = "http://94.245.106.244:5000"  # change to http://router.project-osrm.org for demo server (handles 5000 requests/min)
    url = webserver + query
    contents = requests.get(url=url)
    json_data = contents.json()
    if "message" in json_data:
        if "Too Many Requests" in json_data["message"]:
            raise TooManyRequestsException("Too many requests. OSRM server seems to be overloaded!")
    return json_data


def get_routing_as_json(points):
    points_list = ""
    for p in points[:-1]:
        points_list += str(p[0]) + ',' + str(p[1]) + ';'
    points_list += str(points[-1][0]) + ',' + str(points[-1][1])
    query = "/route/v1/driving/" + points_list + "?steps=true&geometries=polyline&overview=false"
    return get_json_data_from_osrm(query)


def get_routing_as_sorted_list(points):
    json_data = get_routing_as_json(points)
    legs = json_data["routes"][0]["legs"]
    points = []
    for a in legs:
        for x in a["steps"]:
            for y in x["intersections"]:
                points.append(y["location"])
    return points


def get_routing_distance_from_list_of_points(points):
    return get_routing_as_json(points)["routes"][0]["distance"]
