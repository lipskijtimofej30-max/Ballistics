using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts.Infrastructure.Signals
{
    public class SetupDirtyStatusChangedSignal
    {
        public bool IsDirty { get; }
        public SetupDirtyStatusChangedSignal(bool isDirty) => IsDirty = isDirty;
    }
}
