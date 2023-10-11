interface Func<TResult> {
  (signal: AbortSignal): TResult;
}

export function useAbortRequest() {
  let controller: AbortController;
  function abortRequest<TResult>(fnc: Func<TResult>) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();
    const signal = controller.signal;
    return fnc(signal);
  }

  return { abortRequest };
}
