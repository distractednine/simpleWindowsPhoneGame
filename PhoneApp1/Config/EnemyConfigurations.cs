using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1.Config
{
    public class EnemyConfigurations : ViewModelBase
    {
        private String _difficutyLevelString;

        public EnemyConfigurations()
        {
            MoveNumber = 0;
            DescendTime = 5;

            SetMedium();
        }

        public DifficultyLevel DifficultyLevel { get; set; }

        public String DifficutyLevelString
        {
            get { return _difficutyLevelString; }
            set
            {
                if (value.Equals(_difficutyLevelString)) return;
                _difficutyLevelString = value;
                OnPropertyChanged();
            }
        }

        public void ToggleDifficultyLevel()
        {
            switch (DifficultyLevel)
            {
                case (DifficultyLevel.Easy): SetMedium(); return;
                case (DifficultyLevel.Medium): SetHard(); return;
                case (DifficultyLevel.Hard): SetEasy(); return;
            }
        }

        private void SetEasy()
        {
            MoveSpeed = 1800;
            AttackSpeed = 2800;

            DifficultyLevel = DifficultyLevel.Easy;
            UpdateLevelString();
        }

        private void SetMedium()
        {
            MoveSpeed = 1000;
            AttackSpeed = 2000;

            DifficultyLevel = DifficultyLevel.Medium;
            UpdateLevelString();
        }

        private void SetHard()
        {
            MoveSpeed = 700;
            AttackSpeed = 1700;

            DifficultyLevel = DifficultyLevel.Hard;
            UpdateLevelString();
        }

        private void UpdateLevelString()
        {
            DifficutyLevelString = 
                String.Format("Difficulty: {0}", DifficultyLevel);
        }

        public Int32 MoveSpeed { get; set; }
        public Int32 AttackSpeed { get; set; }
        public Int32 MoveNumber { get; set; }
        public Int32 DescendTime { get; set; }
    }
}
