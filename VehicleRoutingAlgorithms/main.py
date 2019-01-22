from __future__ import print_function
from solver import *
from input_parser import *
from constants import *
from brute_force import *
import timeit


def find_routing(coords):
    start = timeit.default_timer()

    # Create Routing Model
    routing = pywrapcp.RoutingModel(len(coords), 1, 0)

    # Define weight of each edge
    distance_callback = create_distance_callback(metricID, coords)
    routing.SetArcCostEvaluatorOfAllVehicles(distance_callback)
    add_distance_dimension(routing, distance_callback)

    # Setting first solution heuristic (cheapest addition).
    search_parameters = pywrapcp.RoutingModel.DefaultSearchParameters()
    search_parameters.first_solution_strategy = routing_enums_pb2.FirstSolutionStrategy.PATH_CHEAPEST_ARC

    # Solve the problem.
    assignment = routing.SolveWithParameters(search_parameters)
    if assignment:
        solution, overallDistance, points = get_solution(coords, routing, assignment)
        t = timeit.default_timer() - start
        print_solution_on_console(solution, overallDistance, t)
        save_to_output_file(vehicleID, points, overallDistance, t)


if __name__ == '__main__':
    vehicleID, metricID = parse_input()
    with open('input' + str(vehicleID) + '.txt') as f:
        locations = [tuple(map(float, i.split(','))) for i in f]
    if metricID != BRUTE_FORCE:
        find_routing(locations)
    else:
        brute_force(locations)
