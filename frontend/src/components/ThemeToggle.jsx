function ThemeToggle({ darkMode, onToggle, ariaLabel, className = "" }) {
  const classes = `theme-toggle ${darkMode ? "is-dark" : ""} ${className}`.trim();

  return (
    <button
      type="button"
      className={classes}
      onClick={onToggle}
      aria-label={ariaLabel}
    />
  );
}

export default ThemeToggle;
