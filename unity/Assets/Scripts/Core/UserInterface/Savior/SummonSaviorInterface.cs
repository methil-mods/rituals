using Core.UserInterface.End;
using Framework.Controller;

namespace Core.UserInterface.Savior
{
    public class SummonSaviorInterface : InterfaceController<SummonSaviorInterface>
    {
        public void LaunchEndGame()
        {
            this.ClosePanel();
            EndController.Instance.LaunchEndGameAnimation();
        }
    }
}