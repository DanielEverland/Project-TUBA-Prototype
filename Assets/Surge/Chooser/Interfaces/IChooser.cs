namespace Surge
{
	interface IChooser
	{
		void Selected();
		void Deselected();
		void Pressed();
		void Released();
	}
}