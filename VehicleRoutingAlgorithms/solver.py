from __future__ import print_function
from ortools.constraint_solver import pywrapcp
from ortools.constraint_solver import routing_enums_pb2
from distances import *
from osrm_handling import *
from helper import *


def create_distance_callback(algorithm_ID, data):
    """Creates callback to return distance between points."""
    _distances = {}

    for from_node in range(len(data)):
        _distances[from_node] = {}
        for to_node in range(len(data)):
            if from_node == to_node:
                _distances[from_node][to_node] = 0
            else:
                _distances[from_node][to_node] = (
                    calculate_distance(algorithm_ID, data[from_node], data[to_node]))

    def distance_callback(from_node, to_node):
        """Returns the distance between the two nodes"""
        return _distances[from_node][to_node]

    return distance_callback


def get_solution(coords, routing, assignment):
    index = routing.Start(0)
    solution = []
    while not routing.IsEnd(index):
        solution.append(routing.IndexToNode(index))
        index = assignment.Value(routing.NextVar(index))
    points = []
    for sol in solution:
        points.append([coords[sol][0], coords[sol][1]])
    distance = get_routing_distance_from_list_of_points(points)

    return solution, distance, points


def print_solution_on_console(solution, distance, t):
    output = 'Route for vehicle'
    for item in solution[0:len(solution)]:
        output += ' {} ->'.format(item)
    output += ' {0}'.format(solution[0])
    output += ' \nNumber of points: {0}'.format(str(len(solution)))
    output += ' \nDistance of route: {0}m\nTime: {1}\n'.format(distance, t)
    print(output)


def save_to_output_file(input_file, points, overall_distance, t):
    list_of_strings = []
    points.append(points[0])
    for p in points:
        list_of_strings.append(pretty_point(p))
    list_of_strings.append(str(overall_distance) + '\n')
    list_of_strings.append(str(t) + '\n')

    intermediate_points = get_routing_as_sorted_list(points)
    list_of_strings.append(pretty_point(points[0]))
    for p in intermediate_points:
        list_of_strings.append(pretty_point(p))
    list_of_strings.append(pretty_point(points[0]))

    with open('output' + str(input_file), 'w') as file:
        file.writelines(list_of_strings)


def add_distance_dimension(routing, distance_callback):
    """Add Global Span constraint"""
    distance = 'Distance'
    maximum_distance = 100000000  # Maximum distance per vehicle - 100000 km.
    routing.AddDimension(
        distance_callback,
        0,  # null slack
        maximum_distance,
        True,
        distance)
    distance_dimension = routing.GetDimensionOrDie(distance)
    distance_dimension.SetGlobalSpanCostCoefficient(100)
