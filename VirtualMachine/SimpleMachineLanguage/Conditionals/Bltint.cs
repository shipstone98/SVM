namespace SVM.SimpleMachineLanguage.Conditionals
{
	public class Bltint : CompareInstruction
	{
		private bool Compare(int a, int b) => a > b;

		public override void Run() => this.Run(this.Compare);
	}
}