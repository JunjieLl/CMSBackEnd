#include "pch.h"
#include "StringEqual.h"

bool __stdcall IsStringEqual(char* s1, char* s2) {
	if (s1 == nullptr || s2 == nullptr) {
		return false;
	}
	while (s1!=nullptr && s2!=nullptr) {
		if (*s1 != *s2) {
			return false;
		}
		++s1;
		++s2;
	}
	if (s1 == nullptr && s2 == nullptr) {
		return true;
	}
	return false;
}