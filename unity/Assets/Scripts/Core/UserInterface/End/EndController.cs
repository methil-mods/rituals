using Framework.Controller;

namespace Core.UserInterface.End
{
    public class EndController : InterfaceController<EndController>
    {
        public void EndGame()
        {
            this.OpenPanel();
        }
    }
}