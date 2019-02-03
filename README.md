# Vehicle-Routing
Web app solving modified [vehicle routing problem](https://en.wikipedia.org/wiki/Vehicle_routing_problem)
(or a set of independent [traveling salesman problems](https://en.wikipedia.org/wiki/Travelling_salesman_problem)). This application
uses [OSRM](http://project-osrm.org/docs/v5.15.2/api/#general-options) API to get all the data. Algorithms are implemented with the
use of [OR-TOOLS](https://developers.google.com/optimization/introduction/python).
## Web app usage
We have a set of vehicles, a set of products and a set of locations that we want to deliver some goods to. All of these variables are
customizable by the user. Difference between this and the original [vehicle routing problem](https://en.wikipedia.org/wiki/Vehicle_routing_problem)
is that the vehicles do not need to have the same starting locations. 

We can create a package by assigning multiple products with the location they should be delivered to.
The user can then select which vehicles should deliver which packages and choose an [algorithm](#python-algorithms) to solve the problem.
## CLI
Command line application can solve [traveling salesman problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem).
It requires an input file with the following [content](/VehicleRoutingAlgorithms/exampleInput.txt) and creates an output file with
the [results](/VehicleRoutingAlgorithms/exampleOutput.txt) (list of ordered locations to visit) according to the [algorithm](#python-algorithms) that was chosen.

Syntax:
```
python main.py INPUT_FILE [ALGORITHM_ID]
```

Current supported algorithms:  
* ID=0 - OSRM+OR-TOOLS
* ID=1 - MANHATTAN+OR-TOOLS
* ID=2 - Brute force

## Python algorithms
* **OSRM+OR-TOOLS** - Suboptimal

&nbsp;&nbsp;&nbsp;&nbsp; Creates a complete graph using OSRM server and then executes OR-TOOLS algorithm.
* **MANHATTAN+OR-TOOLS** - Suboptimal (quicker than the one above)

&nbsp;&nbsp;&nbsp;&nbsp; Creates a complete graph using 
[manhattan metric](https://en.wikipedia.org/wiki/Taxicab_geometry) and then executes OR-TOOLS algorithm.
* **Brute force** - O(n!) complexity

&nbsp;&nbsp;&nbsp;&nbsp; Calculates every permutation of points and gets the shortest.

For more information contact kamil.lepek94@gmail.com
## Requirements to run this app
* Install OR-TOOLS
* Host your own [OSRM server](https://github.com/Project-OSRM/osrm-backend) or change webserver in [this file](/VehicleRoutingAlgorithms/osrm_handling.py) to default.
The default server handles only 5000 requests overall per minute so it might block your requests.
