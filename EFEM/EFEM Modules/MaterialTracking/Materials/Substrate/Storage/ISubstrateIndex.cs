using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFEM.Defines.AtmRobot;
using EFEM.MaterialTracking.LocationServer;

namespace EFEM.MaterialTracking.Utilities
{
    interface ISubstrateIndex
    {
        bool Register(string key, Location loc, Substrate s);
        void Upsert(Substrate s);
        bool Remove(string key);
        bool TryGet(string key, out Substrate s);
        bool TryGetSubstratesAll(out IReadOnlyList<Substrate> substrates);
        bool TryGetAtLoadPort(int portId, int slot, out Substrate s);
        bool TryGetAtRobot(int portId, int slot, out Substrate s);
        bool TryGetSubstratesAtLoadPort(int portId, out IDictionary<int, Substrate> substrates);
        bool TryGetSubstratesAtProcessModule(string pm, out IReadOnlyList<Substrate> substrates);
        bool TryGetSubstrateAtRobot(string rb, RobotArmTypes arm, out Substrate s);
        bool TryGetSubstratesAtRobot(string rb, out IDictionary<int, Substrate> substrates);
    }
}
