import itertools
from osrm_handling import *
from solver import *
import timeit


def brute_force(locations, input_file):
    start = timeit.default_timer()
    spawn = (locations[0],)
    permutations = list(itertools.permutations(locations[1:]))
    dist = 2 * get_routing_distance_from_list_of_points(spawn + permutations[0])
    result = None
    for p in permutations:
        route = spawn + p
        dist_n = get_routing_distance_from_list_of_points(route)
        if dist_n < dist:
            dist = dist_n
            result = route

    print("Distance of route: {0}m".format(dist))
    time = timeit.default_timer() - start
    print("Time: {0}".format(time))
    save_to_output_file(input_file, list(result), dist, time)
