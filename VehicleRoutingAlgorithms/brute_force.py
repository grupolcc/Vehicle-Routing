import itertools
from osrm_handling import *
import timeit


def brute_force(locations):
    start = timeit.default_timer()
    permutations = list(itertools.permutations(locations[1:]))
    dist = 2 * get_routing_distance_from_list_of_points(permutations[0])
    for p in permutations:
        spawn = (locations[0],)
        route = spawn + p
        dist_n = get_routing_distance_from_list_of_points(route)
        if dist_n < dist:
            dist = dist_n
    print("Distance of route: {0}m".format(dist))
    print("Time: {0}".format(timeit.default_timer() - start))
