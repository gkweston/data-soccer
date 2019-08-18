# data-soccer
Automated soccer game to experiment with modeling human behavior in a digital environment, then designing processes that may model similar behaviors without explicit directions. Focusing specifically on teamwork traits. 

Various traits are necessary among human teams to effectively complete tasks as a cohensive unit. If similar traits may be implemented in a digital environment, they may facillitate a more efficient execution of end goals. This project will approach how these traits can effectively be implemented from a computing standpoint. If these processes provide data that can allow for the isolation of trends, then an automated approach will be designed to "train" these traits in a system without explicitely writing code to achieve those ends. Below is a breakdown of this project in its early planning stages:

System layout and Stage 1:
-
A 2D "soccer" field will be created in the Unity Engine with a midfield starting line and goals on each end. To begin, Teams 1 and 2 (simply comprised of small dots) will likely only have three members. As the project developes, team sizes will increase as scripting allows. There will be another small dot to act as the ball, which the players can interact with. Each player will have a stamina counter. If an opposing player challenges the player with the ball, and has greater stamina, he will succesfully "steal" the ball. Once he is within a certain proximity of the goal he will take a shot. Depending on distance and field position there will be a percent chance of success. Upon scoring, that team's score counter will increase. Else, the "ball" will return to the starting line, and player field position will refresh. These simples rules will define how the rest of the project is developed. In this stage the players of each team will be akin to a toddler soccer league, ganging together to attack the ball directly. Once particular team members stamina is worn down, one player will likely have a breakaway giving them a direct shot at the goal.

Stage 2:
-
Team 1's players will be scripted with various code to emulate teamwork traits. These traits and their implementation are as follows:

__Clear direction:__ This primary trait will be the only one shared between both teams in stage one. Otherwise, upon getting the ball, a player from Team 2 would either be stationary, or move randomly about the field.
-If a player does not have the ball, move in the direction of the ball's position
-If a player does have the ball, move in the direction of the opposing team's goal
-If a player with the ball is within proximity of the goal, take a shot

__Communication:__ This will allow Team 1's players to share information with each other
-If a player from Team 1 has the ball, the other players on the team will not move towards the ball
-If passing is implemented, and a player from Team 1 has the ball, the other players will position themselves towards the
goal to receive a pass. Else, the players will not all challenge Team 2's player at once. Instead one will challenge, while
the rest of the team positions themselves to intercept passes (if applicable).

__Defined roles:__ This will implement the basic strategy of player placement and action on the field, as well as position  specific attributes such as greater stamina or speed.
-A goalie position will be created, where that player stays within shooting proximity of their own goal. If a shot is taken  and the goalie is directly in the shooting lane they will have a significant chance of blocking it and reseting the field. Furthermore, if the goalie has greater stamina that the shooter, they will have a larger radius to block the shot. Goaly stamin will be inversly proportional to their distance to their own goal.
-A defender will have a large starting stamin counter, but slower speed. This will allow them to effectively challenge other 
players, but also be outmaneuvered by an opposing player. They will always stay behind midfield.
-A midfielder will have normal stamina and speed, the will be restricted to the edge of shooting range from each goal
-A striker will have greater speed, but less stamina. They will be restricted to the opposing side of midfield
-Each role will have a specific radius from which the can pass to team players. This radius will decrease from defender to 
striker

Collaboration: With the chief goal of scoring on the other team in mind, this trait will be emulated by increasing the liklihood of a defender and midfielder to move towards each other until their radii overlap, allowing for a succesful pass, then the midfielder to the striker.

These traits will be implemented and tested on at a time, until they all can act in coordination with the others.

Stage 3:
-
If all teamwork traits are succesfully implemented to the point that Team 1 perpetually outscores Team 2, then specific data sets will be output by Team 1's players. If specific and useful data can be succesfully isolated to recognize trends, this stage will be focused towards implementing a process (whether through machine learning, or otherwise) to "train" Team 2's players towards emulating the same traits. As a longterm and comprehensive process the overarching goal of this stage will be to implement scripting on Team 2 that will allow them to outscore Team 1 without the explicit scripting of the traits described above.

Supplemental
-
Being in the early stages of this project, some pseudocode has been outlined for the implementation of teamwork traits. Including the overall foundations of the system. (08/18/2019)

These teamwork traits are specifically picked from an [article on effective teamwork skills by Mike Schoultz](https://medium.com/@mikeschoultz/10-team-characteristics-for-effective-teamwork-e0429b362ddd)





