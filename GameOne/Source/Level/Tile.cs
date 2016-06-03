namespace GameOne.Source.Level
{
	class Tile
	{
		// 1x1 square from the playable level
		// Can be either transparent (floor, no collisions), opaque (walls) or clip (mobs collide, projectiles and effects pass trough)
		// Must implement IRenderable, for z-level sorting by the same method as Entities
		// Chlid classes may include active level elements, like doors, buttons or traps
	}
}
