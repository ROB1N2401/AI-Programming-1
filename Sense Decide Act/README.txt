Student: Ivan Fomenko

General info:
	The field is generated each time project is launched. There are 10 sheep and one wolf. The range in which starting health
	is set and how many tiles are filled with grass at the start can be tweaked through constants. 
	
Visuals:
	-Grass is visually depicted through coloured tiles. Brown tiles signify that there is no grass on it. Any other color 
	indicates its health, from green to brownish yellow;
	-Animals are visually depicted through shapes: wolves are rhombs and sheep are circles;
	-Each animal consists of two shapes: outer shape's color indicates in what state the animal currently is (colours are explained in
	"Symbols"). The inner shape shows their current health value in gradient from green to red. 

Behaviour notes:
	-When evading wolves, sheep consider all of the wolves that are hungry within their detection radius, as well as if 
	they themselves are near any walls;
	-Sheep are slower than wolves, however they detect them from farther distance than wolves do;
	-Wolves ignore everything until they catch the sheep they had set on to catch;

Symbols:
	WHITE: animal is wandering;
	PINK: animal has just spawned a new animal;
	ORANGE: animal is moving towards their target;
	CYAN: animal is eating;
	YELLOW: sheep is running away from wolves;
	
	