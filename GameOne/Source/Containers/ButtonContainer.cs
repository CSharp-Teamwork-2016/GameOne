namespace GameOne.Source.Containers
{
    using System.Collections;
    using System.Collections.Generic;
    using Interfaces.MainMenu;

    public class ButtonContainer : IEnumerable
    {
        private List<IButton> buttons;

        public ButtonContainer()
        {
            this.buttons = new List<IButton>();
        }

        public void AddButton(IButton button)
        {
            this.buttons.Add(button);
        }

        //public void RemoveButton(IButton button)
        //{
        //    this.buttons.Remove(button);
        //}
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this.buttons.Count; i++)
            {
                yield return this.buttons[i];
            }
        }

        public void Draw()
        {
            foreach (var button in this.buttons)
            {
                button.Draw();
            }
        }
    }
}
