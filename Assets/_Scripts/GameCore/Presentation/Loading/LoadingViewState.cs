using MVP;
using R3;

namespace GameCore.Presentation.Loading
{
    public class LoadingViewState : ViewState
    {
        public ReactiveCommand CloseCommand { get; } = new();
        public ReactiveCommand OnAnimationDoneCommand { get; } = new();

        public bool IntroAnimationComplete { get; set; }
    }
}