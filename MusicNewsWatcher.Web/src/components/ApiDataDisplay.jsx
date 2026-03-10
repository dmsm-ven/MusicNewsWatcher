import React from "react";

/**
 * @typedef {import("../api/client.ts/models/TrackDownloadHistoryDto").TrackDownloadHistoryDto} TrackDownloadHistoryDto
 */

/**
 * @param {{ downloadHistory: TrackDownloadHistoryDto[] }} props
 */

export default function ApiDataDisplay({ downloadHistory }) {

  const renderDate = (value) => {
    if (!value) return "";
    try {
      const d = value instanceof Date ? value : new Date(value);
      return isNaN(d.getTime()) ? String(value) : d.toLocaleString();
    } catch {
      return String(value);
    }
  };

  const items = Array.isArray(downloadHistory) ? downloadHistory : [];

  return (
    <section className="api-display">
      <h2 className="api-display__title">Download History</h2>

      {items.length === 0 ? (
        <div className="api-display__empty">No download history available</div>
      ) : (
        <div style={{ overflowX: "auto" }}>
          <table className="api-display__table">
            <thead>
              <tr>
                <th>Artist</th>
                <th>Album</th>
                <th>Track</th>
                <th>Started</th>
                <th>Finished</th>
                <th style={{ textAlign: "right" }}>Size (bytes)</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item, idx) => (
                <tr key={idx}>
                  <td>{item?.artistName ?? "-"}</td>
                  <td>{item?.albumName ?? "-"}</td>
                  <td>{item?.trackName ?? "-"}</td>
                  <td>{renderDate(item?.started)}</td>
                  <td>{renderDate(item?.finished)}</td>
                  <td style={{ textAlign: "right" }}>{item?.fileSizeInBytes ?? "-"}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}
