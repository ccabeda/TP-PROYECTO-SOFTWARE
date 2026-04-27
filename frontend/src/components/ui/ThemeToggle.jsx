function ThemeToggle({
  darkMode,
  onToggle,
  ariaLabel,
  className = "",
  disabled = false,
}) {
  const classes = ["theme-toggle", darkMode ? "is-dark" : "", className]
    .filter(Boolean)
    .join(" ");

  return (
    <button
      type="button"
      className={classes}
      onClick={onToggle}
      aria-label={ariaLabel}
      aria-pressed={darkMode}
      disabled={disabled}
    />
  );
}

export default ThemeToggle;
