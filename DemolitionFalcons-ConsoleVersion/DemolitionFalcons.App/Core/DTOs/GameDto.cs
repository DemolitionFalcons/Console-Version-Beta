namespace DemolitionFalcons.App.Core.DTOs
{
    using System;

    public class GameDto
    {
        private string name;

        public GameDto()
        {
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (value.Length < 4 || value.Length > 30)
                {
                    throw new ArgumentException("Game name must be between 4 and 30 symbols!");
                }

                this.name = value;
            }
        }
    }
}

