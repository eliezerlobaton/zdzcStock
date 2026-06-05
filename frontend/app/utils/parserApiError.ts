import type { FetchError } from "ofetch";
import type { ApiErrorResponse } from "~/types";
export function parseApiError(err: unknown, fallback: string): string {
  const fetchError = err as FetchError<ApiErrorResponse>;
  return fetchError?.data?.message ?? fallback;
}
