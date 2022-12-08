using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

namespace TriLib
{
    namespace Samples
    {
        public class DiscrepancySync : RealtimeComponent<DiscrepancySyncModel>
        {

            public string _discrepancyString = "";
            public int _discrepancyInt = 0;

            protected override void OnRealtimeModelReplaced(DiscrepancySyncModel previousModel, DiscrepancySyncModel currentModel)
            {
                if (previousModel != null && _discrepancyString != "")
                {
                    previousModel.discrepancyStringDidChange -= DiscrepancyStringDidChange;
                    previousModel.discrepancyIntDidChange -= DiscrepancyIntDidChange;
                }

                if (currentModel != null && _discrepancyString != "")
                {
                    if (currentModel.isFreshModel)
                        currentModel.discrepancyString = _discrepancyString;
                        currentModel.discrepancyInt = _discrepancyInt;

                    UpdateDiscrepancyState();

                    currentModel.discrepancyStringDidChange += DiscrepancyStringDidChange;
                    currentModel.discrepancyIntDidChange += DiscrepancyIntDidChange;
                }
            }

            private void DiscrepancyIntDidChange(DiscrepancySyncModel model, int value)
            {
                UpdateDiscrepancyState();
            }

            private void DiscrepancyStringDidChange(DiscrepancySyncModel model, string value)
            {
                UpdateDiscrepancyState();
            }

            private void UpdateDiscrepancyState()
            {
                _discrepancyInt = model.discrepancyInt;
                _discrepancyString = model.discrepancyString;
            }

            public void SetDiscrepancyInt(int Int)
            {
                model.discrepancyInt = Int;
            }

            public void SetDiscrepancyString(string String)
            {
                model.discrepancyString = String;
            }
        }

    }
}
