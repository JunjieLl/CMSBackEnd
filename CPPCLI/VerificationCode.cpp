
using namespace System;

namespace INTEROPCPPCLI {
	public ref class VerificationCode {
	public:
		String^ generate() {
			String^ code = "";
			Random^ random = gcnew Random();
			for (int i = 0; i < 6; ++i) {
				code += (random->Next() % 10).ToString();
			}
			return code;
		}
	};
}