using System.Collections.Generic;
using UnityEngine;

namespace Culdcept {
    public class SpecialGround : Ground {
        
        public enum Type {
            Base,     // 城
            Station,  // 砦
            Portal,   // 传送门
            Exchange, // 圣堂
            Temple,   // 祠堂
            Store     // 商店
        }

        public Type type;
    }
}
