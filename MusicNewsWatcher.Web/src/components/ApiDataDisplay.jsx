import React from "react";

export default function ApiDataDisplay({ apiResponseJson }) {
  return (
    <section className="api-display">
      <h2 className="api-display__title">API response</h2>
      <pre className="api-display__body">{apiResponseJson}</pre>
    </section>
  );
}
